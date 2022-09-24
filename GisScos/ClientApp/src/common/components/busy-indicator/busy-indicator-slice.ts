import { createSlice, PayloadAction } from '@reduxjs/toolkit';


const initialState = {
  show: false
}

export const busyIndicatorSlice = createSlice({
  name: 'busyIndicator',
  initialState,
  reducers: {
    showLoading: (state, action: PayloadAction<boolean>) => {
      state.show = action.payload;
    }
  },
});

export const { showLoading } = busyIndicatorSlice.actions;
export default busyIndicatorSlice.reducer;