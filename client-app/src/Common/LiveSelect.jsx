import Select from 'react-select';

const LiveSelect = (props) => {
    const { name, value, values, inputValue, isLoading, validation, onChange, onInputChange} = props;
    return (
        <>
            <Select name={name}
                className='text-start'
                isLoading={isLoading}
                isClearable={true}
                isSearchable={true}
                value={value}
                options={values}
                inputValue={inputValue}
                onChange={onChange}
                onInputChange={onInputChange}
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

export default LiveSelect;