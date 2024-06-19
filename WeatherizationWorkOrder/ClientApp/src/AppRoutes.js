import { Items } from "./components/Items";
import { Users } from "./components/Users";

const AppRoutes = [
  {
    path: '/items',
    element: <Items />
  },
  {
    path: '/work-order',
    element: <Items />
  },
  {
    path: '/users',
    element: <Users />
  }
];

export default AppRoutes;
