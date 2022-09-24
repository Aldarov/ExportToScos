import {
  MiddlewareAPI,
  isPending,
  isFulfilled,
  Middleware,
} from '@reduxjs/toolkit'
import { showLoading } from '@/common/components/busy-indicator';

export const pendingHandlingMiddleware: Middleware = (api: MiddlewareAPI) => (next) => (action) => {
  if (isPending(action)) {
    api.dispatch(showLoading(true));
  }

  if (isFulfilled(action)) {
    api.dispatch(showLoading(false));
  }

  return next(action)
}