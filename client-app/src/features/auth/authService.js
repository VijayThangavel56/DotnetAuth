import api from '../../services/axiosInstance';

export const login = async (credentials) => {
  const response = await api.post('api/login', credentials);
  if (response.data?.token) {
    localStorage.setItem('access_token', response.data.token);
  }
  return response.data;
};

export const logout = () => {
  localStorage.removeItem('access_token');
};
