import React, { useEffect } from 'react';
import Box from '@mui/material/Box';
import { useParams } from "react-router-dom";
import Typography from '@mui/material/Typography';

import { IHeader } from '@/types/common/header';

const ExportResponse: React.FC<IHeader> = ({ setHeader }) => {
  const { id } = useParams();

  useEffect(() => {
    setHeader(
      <Typography variant="h6" color="inherit" component="div">
        Просмотр результата выгрузки пакета с кодом: {id}
      </Typography>
    );
  }, [setHeader, id]);

  return <Box/>
}

export default ExportResponse;