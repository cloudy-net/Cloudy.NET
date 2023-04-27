import { useEffect, useRef } from 'preact/hooks';

export default ({ onClickOutside, children, className, blockDisplay }) => {
    const ref = useRef(null);
  
    useEffect(() => {
      const callback = event => {
        if (!ref.current) {
          return;
        }
        if (!event.target.isConnected) {
          return;
        }
        if (ref.current == event.target || ref.current.contains(event.target)) {
          return;
        }
        onClickOutside();
      };
      document.addEventListener('click', callback);
      return () => document.removeEventListener('click', callback);
    }, []);
  
    return <div className={className} style={className ? "" : blockDisplay ? "display: block;" : "display: inline-block;"} ref={ref}>{children}</div>;
  };