/**
 * Конвертирует Blob-данные в текст
 * @param data Blob-данные
 * @returns
 */
export const convertBlobToString = (data: Blob): Promise<string> => {
  const fileReader = new FileReader();

  return new Promise((resolve, reject) => {
    fileReader.onerror = () => {
      fileReader.abort();
      reject(new Error('Некорректный файл'));
    };

    fileReader.onload = () => {
      resolve(fileReader.result as string);
    };

    fileReader.readAsText(data);
  });
};