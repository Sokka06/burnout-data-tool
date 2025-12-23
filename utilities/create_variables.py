import yaml

FIELDS_TO_QUOTE = {"name", "path", "fileName"}

def long_to_int32(value: int) -> int:
    int32 = value & 0xFFFFFFFF  # mask to 32 bits

    if int32 >= 0x80000000:
        int32 -= 0x100000000

    return int32

def hash_variable(name: str, path: str, file_name: str) -> int:
    from GtHash import calc_gthash

    # Concatenate name + path
    temp = name + path

    # Add "/" if not already present at end
    if len(temp) > 0 and temp[-1] != '/':
        temp += "/"

    # Add file name
    temp += file_name

    # Calculate hash
    return long_to_int32(calc_gthash(temp))

class MyDumper(yaml.Dumper):

    def increase_indent(self, flow=False, indentless=False):
        return super(MyDumper, self).increase_indent(flow, False)
    
    def quoted_selected_fields(dumper, data):
        # Only quote the selected fields
        new_data = {}
        for k, v in data.items():
            if k in FIELDS_TO_QUOTE and isinstance(v, str):
                new_data[k] = yaml.scalarstring.DoubleQuotedScalarString(v)
            else:
                new_data[k] = v
        return dumper.represent_mapping('tag:yaml.org,2002:map', new_data)

if __name__ == "__main__":

    yamlPath = input("Path to template YAML: ")

    with open(yamlPath, "r") as f:
        template_root = yaml.safe_load(f)

    variables = template_root["variables"]
    items = template_root["items"]

    output = {"definitions": {}}

    for v in items:
        for idx, t in enumerate(variables):
            name = t["name"]
            path = t["path"]
            fileName = t["fileName"] % v
            dataType = t["type"]
            hsh = hash_variable(name, path, fileName)

            entry = {
                "name": name,
                "path": path,
                "fileName": fileName,
                "type": dataType
            }

            output["definitions"][hsh] = entry

    outputPath = input("Path to output YAML: ")

    with open(outputPath, "w") as f:
        yaml.dump(output, f, sort_keys=False, default_flow_style=False, Dumper=MyDumper)

    itemCount = len(items)
    variableCount = sum(len(vars_list) for vars_list in output.values())
    print(f'Created {variableCount} variables for {itemCount} items!')