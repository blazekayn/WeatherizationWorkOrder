import { Items } from "./components/Items";
import { WorkOrders } from "./components/WorkOrders";
import { Users } from "./components/Users";

const AppRoutes = [
  {
    path: '/items',
    element: <Items />
  },
  {
    path: '/work-order',
    element: <WorkOrders />
  },
  {
    path: '/users',
    element: <Users />
  }
];

export default AppRoutes;
