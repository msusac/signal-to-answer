const Range = (props) => {

    const { name, onChange, min, max, value, validation } = props

    return (
        <>
            <input className="form-range" 
                type="range"
                name={name} 
                min={min}
                max={max}
                value={value} 
                onChange={onChange} />
            <div className="mt-3 text-center fw-bold">
                {value}
            </div>
            {(validation !== undefined && validation !== '') && (
                <div className="mt-1">
                    <p className="text-danger">{validation}</p>
                </div>
            )}
        </>
    )
}

export default Range;