import Pages from './components/RouterComponent';
import './App.css';
import { BrowserRouter as Router } from 'react-router-dom';

function App() {
  return (
    <>
      <Router>
        <Pages />
      </Router>
    </>
  );
}

export default App;
