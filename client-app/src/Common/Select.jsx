const Select = (props) => {
    const { name, value, values, onChange, validation } = props

    return (
        <>
            <select className="form-select"
                name={name}
                value={value}
                onChange={onChange}
             >
                {values.map((v, i) => (
                    <option value={v.id} key={v.id}>{v.name}</option>
                ))}
             </select>

             {(validation !== undefined && validation !== '') && (
                <div className="mt-3">
                    <p className="text-danger">{validation}</p>
                </div>
            )}
        </>
    )
}

export default Select;