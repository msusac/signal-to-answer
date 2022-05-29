const Textbox = (props) => {
    const { name, type, value, validation, disabled, onChange, placeholder } = props;

    return (
        <>
            <input className="form-control" 
                type={type} 
                name={name} 
                value={value} 
                onChange={onChange} 
                disabled={disabled} 
                placeholder={placeholder} />
            {(validation !== undefined && validation !== '') && (
                <div className="mt-3">
                    <p className="text-danger">{validation}</p>
                </div>
            )}
        </>
    )
}

export default Textbox;