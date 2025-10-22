import { api } from "../configs/axios/interceptors";
import type { ApiResponse } from "../types/ApiResponse";

// Define your types based on the API response
export interface Operation {
  id: number;
  name: string;
  icon: string;
  route: string;
  isActive: boolean;
}

export interface CreateOperationRequest {
  name: string;
  icon: string;
  route: string;
}

export interface UpdateOperationRequest {
  id: number;
  name: string;
  icon: string;
  route: string;
  isActive: boolean;
}

// GET all operations
// Calls: http://93.127.139.74:83/api/v1/operations
export const getAllOperations = async () => {
  const response = await api.get<unknown, ApiResponse<Operation[]>>(
    "/operations",
  );
  return response;
};

// GET operation by ID
// Calls: http://93.127.139.74:83/api/v1/operations/{id}
export const getOperationById = async (id: number) => {
  const response = await api.get<unknown, ApiResponse<Operation>>(
    `/operations/${id}`,
  );
  return response;
};

// POST create new operation
// Calls: http://93.127.139.74:83/api/v1/operations
export const createOperation = async (data: CreateOperationRequest) => {
  const response = await api.post<
    unknown,
    ApiResponse<Operation>,
    CreateOperationRequest
  >("/operations", data);
  return response;
};

// PUT update operation
// Calls: http://93.127.139.74:83/api/v1/operations/{id}
export const updateOperation = async (
  id: number,
  data: UpdateOperationRequest,
) => {
  const response = await api.put<
    unknown,
    ApiResponse<Operation>,
    UpdateOperationRequest
  >(`/operations/${id}`, data);
  return response;
};

// DELETE operation
// Calls: http://93.127.139.74:83/api/v1/operations/{id}
export const deleteOperation = async (id: number) => {
  const response = await api.delete<unknown, ApiResponse<void>>(
    `/operations/${id}`,
  );
  return response;
};
