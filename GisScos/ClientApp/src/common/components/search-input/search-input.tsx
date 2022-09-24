import React, { ChangeEvent, useEffect, useState } from 'react';
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import OutlinedInput from '@mui/material/OutlinedInput';
import CloseIcon from '@mui/icons-material/Close';
import InputAdornment from '@mui/material/InputAdornment';
import IconButton from '@mui/material/IconButton';


const styles = {
  mt: 0.6,
  maxWidth: 700,
  width: '100%'
} as const

type Props = {
  value: string;
  handleChange: (value: string) => void;
  handleClear: (value: string) => void;
  sx?: object;
};

let timer: NodeJS.Timeout;

const SearchInput: React.FC<Props> = props => {
  const css = { ...styles, ...props.sx };
  const [value, setValue] = useState('');

  useEffect(() =>{
    setValue(props.value);
  }, [props.value])

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    setValue(event.target.value);
    clearTimeout(timer);
    timer = setTimeout(() => {
      props.handleChange(event.target.value);
    }, 700)
  }

  const handleClear = () => {
    setValue('');
    props.handleClear('');
  }

  return <FormControl size="small" sx={css}>
    <InputLabel htmlFor="search-input">Поиск</InputLabel>
    <OutlinedInput
      id="search-input"
      value={value}
      onChange={handleChange}
      label='Поиск'
      endAdornment={
        <InputAdornment position="end">
          <IconButton
            onClick={handleClear}
          >
            <CloseIcon />
          </IconButton>
        </InputAdornment>
      }
    />
  </FormControl>
}

export default SearchInput;