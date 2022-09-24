import React, { SyntheticEvent } from 'react';
import MuiAlert, { AlertColor } from '@mui/material/Alert';
import Snackbar from '@mui/material/Snackbar';
import Slide, { SlideProps } from '@mui/material/Slide';

import { useAppDispatch, useAppSelector } from '@/common/hooks/redux-hooks';
import { AlertType } from '@/types/common/alert';
import { hideAlert } from './alert-slice';


type TransitionProps = Omit<SlideProps, 'direction'>;

function TransitionUp(props: TransitionProps) {
  return <Slide {...props} direction="up" />;
}

const Alert: React.FC = () => {
  const dispatch = useAppDispatch();
  const alert = useAppSelector(state => state.common.alert);
  const alertType = alert.info ? AlertType[alert.info.type] as AlertColor : undefined;
  const message = alert.info ? alert.info.message : '';

  const handleClose = (event?: SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') {
      return;
    }

    dispatch(hideAlert());
  }

  return <Snackbar
    open={alert.open}
    onClose={handleClose}
    TransitionComponent={TransitionUp}
  >
    <MuiAlert
      variant="filled"
      onClose={handleClose}
      severity={alertType}
    >
      {message}
    </MuiAlert>
  </Snackbar>;
}

export default Alert;