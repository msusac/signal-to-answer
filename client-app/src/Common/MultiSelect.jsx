import Select from 'react-select';

const MultiSelect = (props) => {
    const { name, value, values, onChange, validation } = props

    return (
        <>
            <Select 
                 className='text-start'
                 name={name}
                 isMulti
                 value={value}
                 options={values}
                 onChange={onChange}
                 getOptionLabel={(option) => option.name}
                 getOptionValue={(option) => option.id}
            />
            {(validation !== undefined && validation !== '') && (
                <div className="mt-3">
                    <p className="text-danger">{validation}</p>
                </div>
            )}
        </>
    )
}

export default MultiSelect;