import axios from 'axios';
import { createBrowserHistory } from 'history';

const api = axios.create({
  baseURL: process.env.REACT_APP_API_BASE_URL || 'https://localhost:7136',
});

// Use history to redirect imperatively (or use react-router's useNavigate inside components)
const history = createBrowserHistory();

api.interceptors.request.use(config => {
  // Don't add token for login request
  if (config.url?.endsWith('/Auth/Login')) {
    return config;
  }

  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
}, error => Promise.reject(error));

api.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 401) {
      // Token expired or unauthorized
      localStorage.removeItem('token');
      // Redirect to login page
      history.push('/login');
      window.location.reload(); // optional to force reload
    }
    return Promise.reject(error);
  }
);

export default api;