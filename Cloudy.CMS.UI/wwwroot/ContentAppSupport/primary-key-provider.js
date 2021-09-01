class PrimaryKeyProvider {
    getFor(content, contentType) {
        return [
            ...contentType.primaryKeys.map(k => content[k])
        ];
    }
}

export default new PrimaryKeyProvider();
