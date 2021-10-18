class PrimaryKeyProvider {
    getFor(content, contentType) {
        let hasValue = false;

        const value = [
            ...contentType.primaryKeys.map(k => {
                if (content[k]) {
                    hasValue = true;
                }

                return content[k];
            })
        ];

        return hasValue ? value : null; // don't return a hollow array
    }
}

export default new PrimaryKeyProvider();
