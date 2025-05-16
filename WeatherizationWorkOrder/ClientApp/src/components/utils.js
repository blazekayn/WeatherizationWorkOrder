export const dedupeMaterials = (materials) => {
    var dedupe = materials.reduce((acc, item) => {
      const key = `${item.description}|${item.costPer}|${item.units}`;

      if (!acc[key]) {
        // First time: initialize, convert id to ids array
        acc[key] = {
          ...item,
          ids: [item.id]
        };
        delete acc[key].id;
      } else {
        // Accumulate amountUsed and ids
        acc[key].amountUsed += item.amountUsed;
        acc[key].ids.push(item.id);
      }

      // Recalculate total every time
      acc[key].total = acc[key].amountUsed * acc[key].costPer;

      return acc;
    }, {});
    dedupe = Object.values(dedupe)
    return dedupe;
}