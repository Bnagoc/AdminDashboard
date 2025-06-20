import axios from 'axios';

const apiClient = axios.create({
    baseURL: 'http://localhost:5000',
});

apiClient.interceptors.request.use(config => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
});

apiClient.interceptors.response.use(
    response => response,
    async error => {
        const originalRequest = error.config;

        if (error?.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;

            try {
                const refreshToken = localStorage.getItem('refreshToken');

                const { data } = await apiClient.post('/auth/refresh', {
                    token: localStorage.getItem('token'),
                    refreshToken
                });

                localStorage.setItem('token', data.token);
                localStorage.setItem('refreshToken', data.refreshToken);

                apiClient.defaults.headers.common['Authorization'] = `Bearer ${data.token}`;
                originalRequest.headers['Authorization'] = `Bearer ${data.token}`;
                console.log(originalRequest);

                return apiClient(originalRequest);
            } catch (refreshError) {
                localStorage.removeItem('token');
                localStorage.removeItem('refreshToken');
                window.location.href = '/login';
                return Promise.reject(refreshError);
            }
        }

        return Promise.reject(error);
    }
);

export default {
    get: (url) => apiClient.get(url),
    post: (url, data) => apiClient.post(url, data),
};