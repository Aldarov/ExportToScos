import {
  MiddlewareAPI,
  isRejectedWithValue,
  Middleware,
} from '@reduxjs/toolkit'
import { showAlert } from '@/common/components/alert/alert-slice';
import { AlertType } from '@/types/common/alert';
import { convertBlobToString } from '@/common/utils/convert-blob-to-string';
import { showLoading } from '@/common/components/busy-indicator';

export const errorHandlingMiddleware: Middleware = (api: MiddlewareAPI) => (next) => async (action) => {
  if (isRejectedWithValue(action)) {
    const data = action.payload.data;
    let message = '';

    if (data instanceof Blob) {
      const file = await convertBlobToString(data);
      message = JSON.parse(file)?.message;
    } else if (data instanceof ArrayBuffer) {
      const json = new TextDecoder().decode(data);
      message = JSON.parse(json)?.message;
    } else {
      message = data.message;
    }

    api.dispatch(showAlert({ message, type: AlertType.error }));
    api.dispatch(showLoading(false));
  }

  return next(action)
}