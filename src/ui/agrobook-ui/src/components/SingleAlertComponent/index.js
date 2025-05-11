import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

const FloatingError = ({ message, kind = 'danger', timeout = 5000, onClose }) => {
  const [show, setShow] = useState(!!message);

  useEffect(() => {
    setShow(!!message);
    
    if (message && timeout) {
      const timer = setTimeout(() => {
        setShow(false);
        if (onClose) onClose();
      }, timeout);
      
      return () => clearTimeout(timer);
    }
  }, [message, timeout, onClose]);

  if (!show) return null;

  // Determine Bootstrap alert class based on kind
  const alertClass = {
    danger: 'alert-danger',
    warning: 'alert-warning',
    success: 'alert-success'
  }[kind] || 'alert-danger';

  // Determine icon based on kind
  const icon = {
    danger: '❌',
    warning: '⚠️',
    success: '✅'
  }[kind] || '❌';

  return (
    <div className="position-fixed top-0 end-0 p-3" style={{ zIndex: 11 }}>
      <div className={`alert ${alertClass} alert-dismissible fade show d-flex align-items-center`} role="alert">
        <span className="me-2" style={{ fontSize: '1.2rem' }}>{icon}</span>
        <div>
          <strong>{kind.charAt(0).toUpperCase() + kind.slice(1)}!</strong> {message}
        </div>
        <button 
          type="button" 
          className="btn-close" 
          onClick={() => {
            setShow(false);
            if (onClose) onClose();
          }}
          aria-label="Close"
        ></button>
      </div>
    </div>
  );
};

FloatingError.propTypes = {
  message: PropTypes.string,
  kind: PropTypes.oneOf(['danger', 'warning', 'success']),
  timeout: PropTypes.number,
  onClose: PropTypes.func,
};

FloatingError.defaultProps = {
  kind: 'danger'
};

export default FloatingError;