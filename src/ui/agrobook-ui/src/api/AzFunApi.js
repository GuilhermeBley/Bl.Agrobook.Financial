import axios from 'axios';

const instance = create();

function create(){
  console.debug('API URL: ' + process.env.REACT_APP_API_BASE_URL);
  return axios.create({
    baseURL: process.env.REACT_APP_API_BASE_URL,
    timeout: 1000 * 60 * 10, // ten minutes
  });
}

export default instance;