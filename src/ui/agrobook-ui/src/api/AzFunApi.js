import axios from 'axios';

const instance = create();

function create() {
  console.debug('API URL: ' + process.env.REACT_APP_API_BASE_URL);
  var i = axios.create({
    baseURL: process.env.REACT_APP_API_BASE_URL,
    timeout: 1000 * 60 * 10, // ten minutes
  });

  // this interceptor use enable you deal with 4xx and 5xx in 'then' promises
  i.interceptors.response.use(response => {
    return response;
  }, error => {
    return error;
  });

  return i;
}

export default instance;