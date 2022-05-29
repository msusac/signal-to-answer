export function isNil(value) {
    return value === null || value === '' || (Array.isArray(value) && value.length === 0)
}

export function isNotNil(value) {
    return !isNil(value)
}

export function validateEqualFields(valueOne, valueTwo, fieldOne, fieldTwo) {
    if (valueOne !== valueTwo) {
        return `Field "${fieldOne}" must match with field "${fieldTwo}".`
    }

    return ''
}

export function validateRange(value, min, max, name) {
    if (value === undefined || value === null || value < min || value > max) {
        return `Field "${name}" must contain between range of ${min} and ${max}.`
    }

    return ''
}

export function validateRequired(value, name) {
    if (value === undefined || value === null || value === '') {
        return `Field "${name}" is required.`
    }

    return ''
}

export function isValid(validations) {
    return Object.keys(validations).filter(key => validations[key] !== '').length === 0
}

export function onChange(properties, event, callback) {
    const target = event.target
    const name = target.name
    const value = target.type === 'checkbox' ? target.checked : target.value

    this.setState(prevState => {
        let temp = prevState

        for (var i = 0; i < properties.length; i++) {
            if (temp[properties[i]] === undefined) {
                temp[properties[i]] = {}
            }

            temp = temp[properties[i]]
        }

        temp[name] = value
        return prevState
    }, () => {
        if (callback !== undefined) {
            callback()
        }
    })
}