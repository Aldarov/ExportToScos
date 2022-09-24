/**
 * Типы предупреждений
 */
export enum AlertType {
  /**
   * Ошибка
   */
  error,
  /**
   * Предупреждение
   */
  warning,
  /**
   * Информация
   */
  info,
  /**
   * Успех
   */
  success
}

/**
 * Информация о предупреждении
 */
export interface IAlertInfo {
  /**
   * Текст сообщения
   */
   message: string;

   /**
    * Тип предупреждения
    */
   type: AlertType;
}

/**
 * Интерфейс для отображения ошибок, предупреждений
 */
export interface IAlert {
  /**
   * если true, то показать сообщение
   */
  open: boolean;

  /**
   * Информация о предупреждении
   */
  info: IAlertInfo | null;
}