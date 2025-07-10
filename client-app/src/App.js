import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import AppRoutes from './routes/AppRoutes';

// App.jsx
function App() {
  return (
    <div style={{ backgroundColor: 'lightblue', minHeight: '100vh' }}>
      <AppRoutes />
      <ToastContainer />
    </div>
  );
}

export default App;  
