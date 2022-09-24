import React, { useState, ReactNode } from 'react';
import {
  BrowserRouter as Router,
  Route,
  Routes
} from "react-router-dom";
import CssBaseline from '@mui/material/CssBaseline';
import Container from '@mui/material/Container';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { ruRU } from '@mui/x-data-grid';
import deepOrange from '@mui/material/colors/deepOrange';

import ExportErrors from '@/features/export-errors/';
import ExportResponse from '@/features/export-response';
import NotFound from '@/common/components/not-found';
import Alert from '@/common/components/alert';
import './app.css';
import BusyIndicator from '@/common/components/busy-indicator';


const theme = createTheme(
  {
    palette: {
      primary: {
        main: '#5c6bc0',
      },
      secondary: deepOrange,
    }
  },
  ruRU
);

const styles = {
  container: {
    my: 2,
    flex: 1,
    overflowY: 'auto',
    display: 'flex',
    flexDirection: 'column'
  }
} as const;

const App: React.FunctionComponent = () => {
  const [header, setHeader] = useState<ReactNode>(null);

  return <ThemeProvider theme={theme}>
    <CssBaseline />
    {
      header &&
      <>
        <AppBar>
          <Toolbar>
            {header}
          </Toolbar>
        </AppBar>
        <Toolbar/>
      </>
    }
    <Container sx={styles.container} maxWidth={false}>
      <Router>
        <Routes>
          <Route path="/" element={<ExportErrors setHeader={setHeader}/>} />
          <Route path="/export-errors" element={<ExportErrors setHeader={setHeader}/>} />
          <Route path="/export-errors/:id" element={<ExportResponse setHeader={setHeader}/>} />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </Router>
    </Container>
    <Alert/>
    <BusyIndicator/>
  </ThemeProvider>;
}

export default App;
