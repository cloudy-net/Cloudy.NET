import { useEffect, useRef } from 'preact/hooks';
import { ComponentChildren } from 'preact';

export default ({ onClickOutside, children, className, blockDisplay } : { onClickOutside: () => void, children: ComponentChildren, className?: string, blockDisplay: boolean }) => {
    const ref = useRef<HTMLDivElement>(null);
  
    useEffect(() => {
      const callback = (event: MouseEvent) => {
        if (!ref.current) {
          return;
        }

        const target = event.target as HTMLElement;
      
        if (!target) {
          return;
        }

        if (!target.isConnected) {
          return;
        }
        if (ref.current == event.target || ref.current.contains(target)) {
          return;
        }
        onClickOutside();
      };
      document.addEventListener('click', callback);
      return () => document.removeEventListener('click', callback);
    }, []);
  
    return <div className={className} style={className ? "" : blockDisplay ? "display: block;" : "display: inline-block;"} ref={ref}>{children}</div>;
  };