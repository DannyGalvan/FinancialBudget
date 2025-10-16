import { Image } from "@heroui/image";
import { motion } from "framer-motion";
import {
  useCallback,
  useEffect,
  useRef,
  useState,
  type RefObject,
} from "react";
import { useMediaQuery } from "react-responsive";
import { Link, useLocation, useNavigate } from "react-router";
import { Images } from "../../assets/images/images";
import { nameRoutes } from "../../configs/constants";
import { useAuth } from "../../hooks/useAuth";
import { Icon } from "../icons/Icon";

const animationConfigPhone = {
  open: {
    x: 0,
    width: "16rem",
    transition: {
      damping: 40,
    },
  },
  closed: {
    x: -250,
    width: 0,
    transition: {
      damping: 40,
      delay: 0.15,
    },
  },
};

const animationConfigDesktop = {
  open: {
    width: "16rem",
    transition: {
      damping: 40,
    },
  },
  closed: {
    width: "4rem",
    transition: {
      damping: 40,
    },
  },
};

export function Sidebar() {
  const isTabletMid = useMediaQuery({ query: "(max-width: 768px)" });
  const [open, setOpen] = useState(
    localStorage.getItem("openSidebar") === "true",
  );
  const sidebarRef = useRef<HTMLDivElement>(null);
  const navigate = useNavigate();
  const { pathname } = useLocation();
  const { logout, email, name, operations } = useAuth();

  useEffect(() => {
    if (isTabletMid) {
      setOpen(false);
      localStorage.setItem("openSidebar", "false");
    }
  }, [isTabletMid]);

  useEffect(() => {
    if (isTabletMid) {
      setOpen(false);
    }
  }, [pathname, isTabletMid]);

  const Nav_animation = isTabletMid
    ? animationConfigPhone
    : animationConfigDesktop;

  const closeSidebar = useCallback(() => {
    setOpen(false);
  }, []);

  const closeSesion = useCallback(() => {
    logout();
    navigate(nameRoutes.login);
  }, [logout, navigate]);

  return (
    <div className="shadow-[13px_2px_22px_4px_rgba(25,123,189,0.3)]">
      <div
        className={`fixed inset-0 z-[47] max-h-screen md:hidden ${
          open ? "block" : "hidden"
        } `}
        onClick={closeSidebar}
      />
      <motion.div
        ref={sidebarRef as RefObject<HTMLDivElement> | null}
        animate={open ? "open" : "closed"}
        className="fixed z-[48] h-screen w-[16rem] max-w-[16rem] overflow-hidden bg-gradient-to-b from-[#0f4264] via-[#197BBD] to-[#1a7bb8] text-white shadow-2xl md:relative transition-all duration-300"
        initial={{ x: isTabletMid ? -250 : 0 }}
        variants={Nav_animation}
        onMouseEnter={() => !isTabletMid && setOpen(true)}
        onMouseLeave={() => !isTabletMid && localStorage.getItem("openSidebar") !== "true" && setOpen(false)}
      >
        <Link
          viewTransition
          className="flex gap-2.5 justify-center items-center py-4 mx-3 font-medium border-b border-white/20 hover:border-white/40 transition-colors duration-300"
          to={nameRoutes.root}
        >
          <Image
            alt="Logo La Sante"
            className="w-100 rounded-xl shadow-lg hover:shadow-blue-500/30 transition-shadow duration-300"
            src={Images.logo}
          />
        </Link>

        <div className="flex h-full flex-col pb-4">
          <ul className="flex flex-col gap-2 overflow-x-hidden px-3 py-6 text-[0.9rem] font-medium scrollbar-thin scrollbar-thumb-white/30 scrollbar-track-transparent">
            <li>
              <Link
                viewTransition
                className={`link transition-all duration-300 ${
                  pathname === nameRoutes.root
                    ? "bg-[#f0f7ff] text-[#197BBD] shadow-lg"
                    : "hover:bg-white/10 hover:text-white hover:shadow-md hover:scale-105"
                }`}
                to={nameRoutes.root}
              >
                <Icon name="bi bi-house-door-fill" size={23} color="#197BBD"/>
                {(open || isTabletMid) && <span>Home</span>}
              </Link>
            </li>
            {open || isTabletMid ? (
              <div className="border-y border-white/20 py-4 my-2">
                <small className="mb-3 inline-block pl-3 text-white/70 font-semibold uppercase tracking-wider text-xs">
                  Opciones
                </small>
                <div className="flex flex-col gap-1">
                  {/* Dynamic Operations from Auth */}
                  {operations?.map((menu) => 
                    menu.operations?.map((operation) => 
                      operation.isVisible && (
                        <Link
                          key={operation.id}
                          viewTransition
                          className={`link transition-all duration-300 capitalize ${
                            pathname.toLowerCase() === operation.path.toLowerCase()
                              ? "bg-[#f0f7ff] text-[#197BBD] shadow-lg"
                              : "hover:bg-white/10 hover:text-white hover:shadow-md hover:scale-105"
                          }`}
                          to={operation.path}
                        >
                          <Icon name={operation.icon} size={20} color={pathname.toLowerCase() === operation.path.toLowerCase() ? "#197BBD" : "currentColor"} />
                          <span>{operation.name}</span>
                        </Link>
                      )
                    )
                  )}
                </div>
              </div>
            ) : null}
          </ul>
          
          {/* Logout Button */}
          <div className="px-3 pb-3 mt-auto pt-5">
            <a 
              className="link font-bold text-red-400 hover:text-red-300 hover:bg-red-500/20 hover:shadow-md hover:scale-105 transition-all duration-300 cursor-pointer flex items-center gap-3" 
              onClick={closeSesion}
            >
              <Icon color="currentColor" name="bi bi-box-arrow-left" size={23} />
              {(open || isTabletMid) && <span>Salir</span>}
            </a>
          </div>

          {/* User Info Section */}
          {open ? (
            <div className="px-3 pb-12">
              <div className="flex items-center gap-3 p-3">
                <div className="flex-shrink-0">
                  <Icon name="bi bi-person-circle" size={40} color="197BBD" />
                </div>
                <div className="flex-1 min-w-0">
                  <p className="text-sm font-bold text-white truncate">
                    {name}
                  </p>
                  <p className="text-xs text-white/80 truncate">
                    {email}
                  </p>
                </div>
              </div>
            </div>
          ) : null}
        </div>
      </motion.div>
    </div>
  );
}
