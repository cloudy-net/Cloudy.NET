declare module "*.svg" {
  import { FunctionComponent, SVGProps } from "preact";

  const ReactComponent: FunctionComponent<
    SVGProps<SVGSVGElement> & { title?: string }
  >;

  export default ReactComponent;
}
