function arrayEquals(a, b) {
    if (a == null && b == null) {
        return true;
    }

    if (a == null) {
        return false;
    }

    if (b == null) {
        return false;
    }

    return a.every((ai, i) => ai === b[i]);
}

export default arrayEquals;