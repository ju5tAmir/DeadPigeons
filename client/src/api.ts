/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface BoardDetails {
  /** @format uuid */
  boardId?: string;
  packageDetails?: PackageResponse;
  playSequence?: number[] | null;
  /** @format double */
  prize?: number;
}

export interface BoardRequest {
  /** @format uuid */
  gameId?: string;
  /** @format uuid */
  packageId?: string;
  /** @uniqueItems true */
  playSequence?: number[] | null;
}

export interface BoardResponse {
  /** @format uuid */
  boardId?: string;
  game?: GameResponse;
  package?: PackageResponse;
  /** @uniqueItems true */
  playSequence?: number[] | null;
  /** @format date-time */
  playTime?: string | null;
}

export interface CreateTransactionRequest {
  /** @format uuid */
  userId?: string;
  paymentMethod?: string | null;
  transactionType?: string | null;
  /** @format double */
  amount?: number;
}

export interface FinishGameRequest {
  /** @format uuid */
  gameId?: string;
  /** @uniqueItems true */
  winningSequence?: number[] | null;
}

export interface GameResponse {
  /** @format uuid */
  gameId?: string;
  /** @format int32 */
  weekNumber?: number;
  /** @format date-time */
  validFromDate?: string;
  /** @format date-time */
  validUntilDate?: string;
  status?: string | null;
  winningSequence?: number[] | null;
  /** @format date-time */
  finishedAt?: string | null;
}

export interface LoginRequest {
  email?: string | null;
  password?: string | null;
}

export interface LoginResponse {
  jwt?: string | null;
}

export interface PackageRequest {
  /** @format int32 */
  numberOfFields?: number;
  /** @format double */
  price?: number;
}

export interface PackageResponse {
  /** @format uuid */
  packageId?: string;
  /** @format int32 */
  numberOfFields?: number;
  /** @format double */
  price?: number;
}

export interface PlayerDetails {
  /** @format uuid */
  playerId?: string;
  firstName?: string | null;
  lastName?: string | null;
  username?: string | null;
}

export interface PreferenceResponse {
  ifBalanceIsNegative?: boolean | null;
  ifPlayerWon?: boolean | null;
  notificationType?: string | null;
}

export interface PreferenceUpdateRequest {
  ifBalanceIsNegative?: boolean;
  ifPlayerWon?: boolean;
  notificationType?: string | null;
}

export interface RegisterRequest {
  firsName?: string | null;
  lastName?: string | null;
  username?: string | null;
  email?: string | null;
  phoneNumber?: string | null;
  password?: string | null;
}

export interface RegisterResponse {
  userId?: string | null;
}

export interface TransactionResponse {
  /** @format uuid */
  transactionId?: string;
  paymentMethod?: string | null;
  transactionType?: string | null;
  /** @format double */
  amount?: number;
  status?: string | null;
  /** @format date-time */
  transactionDate?: string | null;
}

export interface UserInfo {
  userId?: string | null;
  firstName?: string | null;
  lastName?: string | null;
  username?: string | null;
  email?: string | null;
  phoneNumber?: string | null;
  role?: string | null;
  isActive?: boolean;
  isAutoplay?: boolean;
  /** @format date-time */
  registerationDate?: string;
}

export interface UserInfoResponse {
  userInfo?: UserInfo;
  userPreference?: PreferenceResponse;
}

export interface Winners {
  playerDetails?: PlayerDetails;
  boardDetails?: BoardDetails[] | null;
  /** @format double */
  totalPrize?: number;
}

export interface WinnersRequest {
  /** @format uuid */
  gameId?: string;
  /** @uniqueItems true */
  winningSequence?: number[] | null;
}

export interface WinnersResponse {
  gameDetails?: GameResponse;
  winners?: Winners[] | null;
}

import type { AxiosInstance, AxiosRequestConfig, AxiosResponse, HeadersDefaults, ResponseType } from "axios";
import axios from "axios";

export type QueryParamsType = Record<string | number, any>;

export interface FullRequestParams extends Omit<AxiosRequestConfig, "data" | "params" | "url" | "responseType"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseType;
  /** request body */
  body?: unknown;
}

export type RequestParams = Omit<FullRequestParams, "body" | "method" | "query" | "path">;

export interface ApiConfig<SecurityDataType = unknown> extends Omit<AxiosRequestConfig, "data" | "cancelToken"> {
  securityWorker?: (
    securityData: SecurityDataType | null,
  ) => Promise<AxiosRequestConfig | void> | AxiosRequestConfig | void;
  secure?: boolean;
  format?: ResponseType;
}

export enum ContentType {
  Json = "application/json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public instance: AxiosInstance;
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private secure?: boolean;
  private format?: ResponseType;

  constructor({ securityWorker, secure, format, ...axiosConfig }: ApiConfig<SecurityDataType> = {}) {
    this.instance = axios.create({ ...axiosConfig, baseURL: axiosConfig.baseURL || "" });
    this.secure = secure;
    this.format = format;
    this.securityWorker = securityWorker;
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected mergeRequestParams(params1: AxiosRequestConfig, params2?: AxiosRequestConfig): AxiosRequestConfig {
    const method = params1.method || (params2 && params2.method);

    return {
      ...this.instance.defaults,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...((method && this.instance.defaults.headers[method.toLowerCase() as keyof HeadersDefaults]) || {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected stringifyFormItem(formItem: unknown) {
    if (typeof formItem === "object" && formItem !== null) {
      return JSON.stringify(formItem);
    } else {
      return `${formItem}`;
    }
  }

  protected createFormData(input: Record<string, unknown>): FormData {
    if (input instanceof FormData) {
      return input;
    }
    return Object.keys(input || {}).reduce((formData, key) => {
      const property = input[key];
      const propertyContent: any[] = property instanceof Array ? property : [property];

      for (const formItem of propertyContent) {
        const isFileType = formItem instanceof Blob || formItem instanceof File;
        formData.append(key, isFileType ? formItem : this.stringifyFormItem(formItem));
      }

      return formData;
    }, new FormData());
  }

  public request = async <T = any, _E = any>({
    secure,
    path,
    type,
    query,
    format,
    body,
    ...params
  }: FullRequestParams): Promise<AxiosResponse<T>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const responseFormat = format || this.format || undefined;

    if (type === ContentType.FormData && body && body !== null && typeof body === "object") {
      body = this.createFormData(body as Record<string, unknown>);
    }

    if (type === ContentType.Text && body && body !== null && typeof body !== "string") {
      body = JSON.stringify(body);
    }

    return this.instance.request({
      ...requestParams,
      headers: {
        ...(requestParams.headers || {}),
        ...(type ? { "Content-Type": type } : {}),
      },
      params: query,
      responseType: responseFormat,
      data: body,
      url: path,
    });
  };
}

/**
 * @title API
 * @version 1.0
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
  api = {
    /**
     * No description
     *
     * @tags Auth
     * @name AuthRegisterCreate
     * @request POST:/api/auth/register
     * @secure
     */
    authRegisterCreate: (data: RegisterRequest, params: RequestParams = {}) =>
      this.request<RegisterResponse, any>({
        path: `/api/auth/register`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Auth
     * @name AuthLoginCreate
     * @request POST:/api/auth/login
     * @secure
     */
    authLoginCreate: (data: LoginRequest, params: RequestParams = {}) =>
      this.request<LoginResponse, any>({
        path: `/api/auth/login`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Auth
     * @name AuthLogoutList
     * @request GET:/api/auth/logout
     * @secure
     */
    authLogoutList: (params: RequestParams = {}) =>
      this.request<void, any>({
        path: `/api/auth/logout`,
        method: "GET",
        secure: true,
        ...params,
      }),

    /**
     * No description
     *
     * @tags Auth
     * @name AuthUserinfoList
     * @request GET:/api/auth/userinfo
     * @secure
     */
    authUserinfoList: (params: RequestParams = {}) =>
      this.request<UserInfoResponse, any>({
        path: `/api/auth/userinfo`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Board
     * @name BoardDetail
     * @request GET:/api/board/{id}
     * @secure
     */
    boardDetail: (id: string, params: RequestParams = {}) =>
      this.request<BoardResponse, any>({
        path: `/api/board/${id}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Board
     * @name BoardDelete
     * @request DELETE:/api/board/{id}
     * @secure
     */
    boardDelete: (id: string, params: RequestParams = {}) =>
      this.request<boolean, any>({
        path: `/api/board/${id}`,
        method: "DELETE",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Board
     * @name BoardUpdate
     * @request PUT:/api/board/{id}
     * @secure
     */
    boardUpdate: (id: string, data: BoardRequest, params: RequestParams = {}) =>
      this.request<BoardResponse, any>({
        path: `/api/board/${id}`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Board
     * @name BoardAllList
     * @request GET:/api/board/all
     * @secure
     */
    boardAllList: (params: RequestParams = {}) =>
      this.request<BoardResponse[], any>({
        path: `/api/board/all`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Board
     * @name BoardGameDetail
     * @request GET:/api/board/game/{id}
     * @secure
     */
    boardGameDetail: (id: string, params: RequestParams = {}) =>
      this.request<BoardResponse[], any>({
        path: `/api/board/game/${id}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Board
     * @name BoardPlayCreate
     * @request POST:/api/board/play
     * @secure
     */
    boardPlayCreate: (data: BoardRequest, params: RequestParams = {}) =>
      this.request<BoardResponse, any>({
        path: `/api/board/play`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Game
     * @name GameList
     * @request GET:/api/game
     * @secure
     */
    gameList: (params: RequestParams = {}) =>
      this.request<GameResponse, any>({
        path: `/api/game`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Game
     * @name GameStartList
     * @request GET:/api/game/start
     * @secure
     */
    gameStartList: (params: RequestParams = {}) =>
      this.request<GameResponse, any>({
        path: `/api/game/start`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Game
     * @name GameFinishCreate
     * @request POST:/api/game/finish
     * @secure
     */
    gameFinishCreate: (data: FinishGameRequest, params: RequestParams = {}) =>
      this.request<GameResponse, any>({
        path: `/api/game/finish`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Package
     * @name PackageAllList
     * @request GET:/api/package/all
     * @secure
     */
    packageAllList: (params: RequestParams = {}) =>
      this.request<PackageResponse[], any>({
        path: `/api/package/all`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Package
     * @name PackageDetail
     * @request GET:/api/package/{id}
     * @secure
     */
    packageDetail: (id: string, params: RequestParams = {}) =>
      this.request<PackageResponse, any>({
        path: `/api/package/${id}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Package
     * @name PackageDelete
     * @request DELETE:/api/package/{id}
     * @secure
     */
    packageDelete: (id: string, params: RequestParams = {}) =>
      this.request<boolean, any>({
        path: `/api/package/${id}`,
        method: "DELETE",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Package
     * @name PackageCreate
     * @request POST:/api/package
     * @secure
     */
    packageCreate: (data: PackageRequest, params: RequestParams = {}) =>
      this.request<PackageResponse, any>({
        path: `/api/package`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Preference
     * @name PreferencesDetail
     * @request GET:/api/preferences/{userId}
     * @secure
     */
    preferencesDetail: (userId: string, params: RequestParams = {}) =>
      this.request<PreferenceResponse, any>({
        path: `/api/preferences/${userId}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Preference
     * @name PreferencesUpdate
     * @request PUT:/api/preferences/{userId}
     * @secure
     */
    preferencesUpdate: (userId: string, data: PreferenceUpdateRequest, params: RequestParams = {}) =>
      this.request<PreferenceResponse, any>({
        path: `/api/preferences/${userId}`,
        method: "PUT",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Transaction
     * @name TransactionDetail
     * @request GET:/api/transaction/{id}
     * @secure
     */
    transactionDetail: (id: string, params: RequestParams = {}) =>
      this.request<TransactionResponse, any>({
        path: `/api/transaction/${id}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Transaction
     * @name TransactionAllList
     * @request GET:/api/transaction/all
     * @secure
     */
    transactionAllList: (params: RequestParams = {}) =>
      this.request<TransactionResponse[], any>({
        path: `/api/transaction/all`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Transaction
     * @name TransactionCreateCreate
     * @request POST:/api/transaction/create
     * @secure
     */
    transactionCreateCreate: (data: CreateTransactionRequest, params: RequestParams = {}) =>
      this.request<TransactionResponse, any>({
        path: `/api/transaction/create`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Winners
     * @name WinnerDetail
     * @request GET:/api/winner/{id}
     * @secure
     */
    winnerDetail: (id: string, params: RequestParams = {}) =>
      this.request<WinnersResponse, any>({
        path: `/api/winner/${id}`,
        method: "GET",
        secure: true,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Winners
     * @name WinnerCreate
     * @request POST:/api/winner
     * @secure
     */
    winnerCreate: (data: WinnersRequest, params: RequestParams = {}) =>
      this.request<WinnersResponse, any>({
        path: `/api/winner`,
        method: "POST",
        body: data,
        secure: true,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),
  };
}
