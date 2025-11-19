export enum ApiResponseStatus {
  Success = 'Success',
  Error = 'Error'
}

export interface ApiResponse<T> {
  status: ApiResponseStatus;
  message?: string;
  data?: T;
  correlationId?: string;
  errorCode?: number;
}
