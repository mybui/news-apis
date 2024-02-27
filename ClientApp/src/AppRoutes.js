import { Home } from "./components/Home";
import { News } from "./components/News";

const AppRoutes = [
  {
    index: true,
    element: <Home />,
  },
  {
    path: "/news",
    element: <News />,
  },
];

export default AppRoutes;
