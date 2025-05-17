using Moq;
using WeatherizationWorkOrder.Business;
using WeatherizationWorkOrder.Data.Interfaces;
using WeatherizationWorkOrder.Models;

namespace WeatherizationWorkOrder.Tests
{
    [TestClass]
    public class WorkOrderProviderTests
    {
        private Mock<IWorkOrderDataProvider> _workOrderData;
        private Mock<IInventoryDataProvider> _inventoryData;
        private WorkOrderProvider _sut;

        [TestInitialize]
        public void SetUp()
        {
            _workOrderData = new Mock<IWorkOrderDataProvider>();
            _inventoryData = new Mock<IInventoryDataProvider>();
            _sut = new WorkOrderProvider(_workOrderData.Object, _inventoryData.Object);
        }

        [TestMethod]
        public async Task CreateWorkOrder_sets_missing_PreparedDate_and_audit_fields_then_calls_DataProvider()
        {
            var wo = new WorkOrder { PreparedDate = DateTime.MinValue, PreparedBy = "tester" };
            _workOrderData.Setup(d => d.Create(It.IsAny<WorkOrder>())).ReturnsAsync(42);

            var id = await _sut.CreateWorkOrder(wo);

            Assert.AreEqual(42, id);
            _workOrderData.Verify(d => d.Create(It.Is<WorkOrder>(w =>
                w.PreparedDate != DateTime.MinValue &&
                w.LastModifiedBy == "tester" &&
                (DateTime.Now - w.LastModified) < TimeSpan.FromSeconds(1))), Times.Once);
        }

        [TestMethod]
        public async Task GetAllWorkOrders_bool_populates_sub_properties_and_costs()
        {
            var list = new List<WorkOrder> { new() { Id = 1 } };
            _workOrderData.Setup(d => d.Read(true)).ReturnsAsync(list);
            _workOrderData.Setup(d => d.ReadMaterials(1)).ReturnsAsync(
            [
                new() { Total = 5m }, new() { Total = 7m }
            ]);
            _workOrderData.Setup(d => d.ReadLabors(1)).ReturnsAsync(
            [
                new() { Total = 10m }
            ]);

            var result = await _sut.GetAllWorkOrders(true);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(12m, result[0].MaterialCost);
            Assert.AreEqual(10m, result[0].LaborCost);
            Assert.AreEqual(22m, result[0].TotalCost);
        }

        [TestMethod]
        public async Task AddMaterial_returns_failure_when_inventory_has_none()
        {
            var request = new AddMaterialRequest { Description = "Wire", Units = "ft", Used = 10 };
            _inventoryData.Setup(i => i.ReadByDesc("Wire", "ft"))
                          .ReturnsAsync([]);

            var res = await _sut.AddMaterialToWorkOrder(request);

            Assert.IsFalse(res.Success);
            Assert.AreEqual("None of this material in inventory.", res.Message);
        }

        [TestMethod]
        public async Task AddMaterial_returns_failure_when_not_enough_units()
        {
            var request = new AddMaterialRequest { Description = "Wire", Units = "ft", Used = 10 };
            _inventoryData.Setup(i => i.ReadByDesc("Wire", "ft"))
                          .ReturnsAsync([ new() { Remaining = 3 } ]);

            var res = await _sut.AddMaterialToWorkOrder(request);

            Assert.IsFalse(res.Success);
            StringAssert.Contains(res.Message, "Only 3 units");
        }

        [TestMethod]
        public async Task AddMaterial_succeeds_and_updates_all_inventories_and_workOrder()
        {
            var items = new List<InventoryItem>
            {
                new() { Id = 1, Remaining = 4 },
                new() { Id = 2, Remaining = 6 }
            };
            var req = new AddMaterialRequest { WoId = 99, Description = "Wire", Units = "ft", Used = 7 };

            _inventoryData.Setup(i => i.ReadByDesc("Wire", "ft")).ReturnsAsync(items);
            _workOrderData.Setup(i => i.ReadMaterials(99)).ReturnsAsync([]);
            _inventoryData.Setup(i => i.Update(It.IsAny<InventoryItem>())).Returns(Task.CompletedTask);
            _workOrderData.Setup(i => i.AddMaterial(It.IsAny<int>(), 99, It.IsAny<decimal>()))
                          .Returns(Task.CompletedTask);

            var res = await _sut.AddMaterialToWorkOrder(req);

            Assert.IsTrue(res.Success);
            _inventoryData.Verify(i => i.Update(It.Is<InventoryItem>(inv => inv.Id == 1 && inv.Remaining == 0)), Times.Once);
            _inventoryData.Verify(i => i.Update(It.Is<InventoryItem>(inv => inv.Id == 2 && inv.Remaining == 3)), Times.Once);
            _workOrderData.Verify(i => i.AddMaterial(1, 99, 4), Times.Once);
            _workOrderData.Verify(i => i.AddMaterial(2, 99, 3), Times.Once);
        }

        [TestMethod]
        public async Task DeleteMaterials_calls_DeleteMaterial_for_each_id_and_returns_last_state()
        {
            _workOrderData.Setup(d => d.ReadMaterial(It.IsAny<int>()))
                          .ReturnsAsync(new Material { Id = 1, InventoryItemId = 1, WorkOrderId = 99, AmountUsed = 1 });
            _inventoryData.Setup(i => i.Read(1)).ReturnsAsync(new InventoryItem { Id = 1 });
            _workOrderData.Setup(d => d.ReadMaterials(99)).ReturnsAsync([]);

            var res = await _sut.DeleteMaterials([ 10, 11, 12 ]);

            _workOrderData.Verify(d => d.DeleteMaterial(10), Times.Once);
            _workOrderData.Verify(d => d.DeleteMaterial(11), Times.Once);
            _workOrderData.Verify(d => d.DeleteMaterial(12), Times.Once);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public async Task DeleteLabor_deletes_and_returns_remaining_labors()
        {
            var labor = new Labor { Id = 1, WorkOrderId = 42 };
            _workOrderData.Setup(d => d.ReadLabor(1)).ReturnsAsync(labor);
            _workOrderData.Setup(d => d.DeleteLabor(1)).Returns(Task.CompletedTask);
            _workOrderData.Setup(d => d.ReadLabors(42)).ReturnsAsync([]);

            var res = await _sut.DeleteLabor(1);

            _workOrderData.Verify(d => d.DeleteLabor(1), Times.Once);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public async Task UpdateWorkOrder_sets_LastModified_and_calls_data_layer()
        {
            var wo = new WorkOrder { Id = 1 };

            await _sut.UpdateWorkOrder(wo);

            _workOrderData.Verify(d => d.Update(It.Is<WorkOrder>(w =>
                (DateTime.Now - w.LastModified) < TimeSpan.FromSeconds(1))), Times.Once);
        }

        [TestMethod]
        public async Task DeleteLabor_throws_when_bad_id()
        {
            _workOrderData.Setup(d => d.ReadLabor(555)).ReturnsAsync((Labor)null!);

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _sut.DeleteLabor(555));
        }

        [TestMethod]
        public async Task DeleteMaterial_throws_when_bad_id()
        {
            _workOrderData.Setup(d => d.ReadMaterial(123)).ReturnsAsync((Material)null!);

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _sut.DeleteMaterial(123));
        }
    }
}
