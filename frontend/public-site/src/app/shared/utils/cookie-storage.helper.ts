export function cookiesStorageRoot() {
  return {
    getItem: (key: string): string | null => {
      const match = document.cookie.match(new RegExp('(^|; )' + key + '=([^;]*)'));
      return match ? decodeURIComponent(match[2]) : null;
    },
    setItem: (key: string, value: string): void => {
      document.cookie = `${encodeURIComponent(key)}=${encodeURIComponent(value)}; path=/;`;
    },
    removeItem: (key: string): void => {
      document.cookie = `${encodeURIComponent(key)}=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/;`;
    }
  };
}
