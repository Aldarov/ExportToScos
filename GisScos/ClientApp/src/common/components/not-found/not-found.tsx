import React from 'react';
import Button from '@mui/material/Button';
import Box from '@mui/material/Box';
import { Link } from 'react-router-dom';
import HomeIcon from '@mui/icons-material/Home';

const styles = {
  root: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center'
  }
} as const;

const ExportErrors: React.FC = () => {
  return <Box sx={styles.root}>
    <h1>404</h1>
    <p>К сожалению, страница не найдена.</p>
    <Button
      variant='contained'
      component={Link}
      to='/'
    >
      <HomeIcon />
      На главную
    </Button>
  </Box>
}

export default ExportErrors;