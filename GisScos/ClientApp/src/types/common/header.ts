import { ReactNode } from "react";

/**
 * Интерфейс для установки шапки приложения
 */
export interface IHeader {
  /**
   * Устанавливает шапку приложения
   * @param header
   */
  setHeader(header: ReactNode): void;
}