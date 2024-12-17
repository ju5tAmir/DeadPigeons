import { useNavigate } from "react-router-dom";
import { atom, useAtom } from "jotai";
import {UserInfo} from "../api";
import { http } from "../http";
import { atomWithStorage, createJSONStorage } from "jotai/utils";
import {getRoleFromJWT} from "../utils/JwtUtils.ts";

// Storage key for JWT
export const TOKEN_KEY = "token";
export const tokenStorage = createJSONStorage<string | null>(
    () => sessionStorage,
);
export const jwtAtom = atomWithStorage<string | null>(TOKEN_KEY, null, tokenStorage);

export const isAuth = atom<boolean | null>(null);

export const checkAuth = atom((get) => {
    const token = get(jwtAtom);
    if (token) {
      return getRoleFromJWT(token);
    }
    return null;
});


export const userInfoAtom = atom(async (get) => {
  // Create a dependency on 'token' atom
  const token = get(jwtAtom);
  if (!token)
  {
    get(isAuth)
    return null;
  }
  // Fetch user-info
  const response = await http.authUserinfoList();
  get(isAuth)
  return response.data;
});

export type Credentials = { email: string; password: string };

type AuthHook = {
  user: UserInfo | null;
  login: (credentials: Credentials) => Promise<void>;
  logout: () => void;
};

export const useAuth = () => {
  const [_, setJwt] = useAtom(jwtAtom);
  const [user] = useAtom(userInfoAtom);
  const navigate = useNavigate();

  const login = async (credentials: Credentials) => {
    const response = await http.authLoginCreate(credentials);
    const data = response.data;
    setJwt(data.jwt!);
  };

  const logout = async () => {
    setJwt(null);
    navigate("/login");
  };

  return {
    user,
    login,
    logout,
  } as AuthHook;
};