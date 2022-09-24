export type ExportError = {
  /**
   * Идентификатор ошибки
   */
  id: number;

  /**
   * Идентификатор сущности
   */
  entityId: number;

  /**
   * Имя сущности
   */
  entity: string;

  /**
   * Вариант действия
   * */
  actionName: string;

  /**
   * Идентификатор записи сущности в системе Университет
   */
  externalId: number;

  /**
   * Идентификатор записи сущности в ГИС СЦОС
   */
  scosId: number;

  /**
   * Идентификатор ошибки, общий для всех записей с одинаковым ExternalId и ActionName
   * */
  errorId: string;

  /**
   * Идентификатор пакета, при помощи кот. была экспортирована данная запись
   */
  exportQueueId: number;

  /**
   * Сообщение об ошибке
   */
  message: string;

  /**
   * Json с данными по ошибке
   */
  json: string;

  /**
   * Дата получения ошибки
   */
  createDate: Date;

  /**
   * Если true, то ошибке исправлена
   */
  resolved: boolean;
}