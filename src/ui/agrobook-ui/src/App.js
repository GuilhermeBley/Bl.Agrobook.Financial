import Pages from './components/Pages';
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
