import { RefObject, useEffect, useRef } from "react";

interface ObserveOptions {
  containerRef?: RefObject<Element>;
  callback: (isIntersecting: boolean) => void;
  threshold: number;
  rootMargin?: string;
}

function useObserve({
  containerRef,
  callback,
  threshold,
  rootMargin,
}: ObserveOptions) {
  const ref = useRef(null);

  useEffect(() => {
    const childElement = ref.current;
    const parentElement = containerRef?.current;

    const innerCallback = (entries: IntersectionObserverEntry[]) => {
      const [entry] = entries;
      callback(entry.isIntersecting);
    };

    const observer = new IntersectionObserver(innerCallback, {
      root: parentElement,
      rootMargin: rootMargin ?? "0px",
      threshold: threshold,
    });

    if (childElement) {
      observer.observe(childElement);
    }

    return () => {
      if (childElement) {
        observer.unobserve(childElement);
      }
    };
  }, [callback, containerRef, rootMargin, threshold]);

  return ref;
}

export default useObserve;
