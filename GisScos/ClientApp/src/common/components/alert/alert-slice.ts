import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { IAlert, IAlertInfo } from '@/types/common/alert';


const initialState: IAlert = {
  info: null,
  open: false
}

export const alertSlice = createSlice({
  name: 'alert',
  initialState,
  reducers: {
    showAlert: (state, action: PayloadAction<IAlertInfo>) => {
      state.info = action.payload;
      state.open = true;
    },
    hideAlert: (state) => {
      state.open = false;
    }
  },
});

export const { showAlert, hideAlert } = alertSlice.actions;
export default alertSlice.reducer;