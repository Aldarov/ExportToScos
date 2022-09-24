import { GridValueGetterFullParams, GridValueGetterParams } from "@mui/x-data-grid";


export default function gridDateGetter(props: GridValueGetterParams): Date {
  const value = (props as GridValueGetterFullParams).value;
  return value && new Date(value);
};