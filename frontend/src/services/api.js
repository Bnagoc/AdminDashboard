import axios from 'axios';

const apiClient = axios.create({
    baseURL: 'http://localhost:5000',
});

export default {
    get: (url) => apiClient.get(url),
    post: (url, data) => apiClient.post(url, data),
};