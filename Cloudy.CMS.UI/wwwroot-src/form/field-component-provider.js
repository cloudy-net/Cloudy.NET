import FieldComponentContext from "./field-component-context.js";

export default ({ children }) => {
    return <FieldComponentContext.Provider value="Bob">
        {children}
    </FieldComponentContext.Provider>;
};