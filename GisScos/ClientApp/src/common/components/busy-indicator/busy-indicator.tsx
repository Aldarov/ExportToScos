import React from 'react';
import Modal from '@mui/material/Modal';

import { useAppSelector } from '@/common/hooks/redux-hooks';
import './busy-indicator.css';


const BusyIndicator: React.FC = () => {
  const show = useAppSelector(state => state.common.busyIndicator.show);

  return <Modal
    open={show}
    className='spinner'
    disableEnforceFocus
    disableAutoFocus
  >
    <div className="loader">Загрузка...</div>
  </Modal>;
}

export default BusyIndicator;