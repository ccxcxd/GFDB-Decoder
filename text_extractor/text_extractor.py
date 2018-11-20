import os
from subprocess import call
import unitypack
import json
from collections import OrderedDict

ADB = r"C:\Program Files\Nox\bin\nox_adb.exe";

servers = {
    "zh": "/sdcard/Android/data/com.digitalsky.girlsfrontline.cn",
    "zh-hant": "/data/data/tw.txwy.and.snqx",
    "ko": "/sdcard/Android/data/kr.txwy.and.snqx",
    "en": "/sdcard/Android/data/com.sunborn.girlsfrontline.en",
    "ja": "/sdcard/Android/data/com.sunborn.girlsfrontline.jp",
};

with open('table_files.json', 'r', encoding="utf-8") as f:
    table_files = json.load(f);

def extract_all():
    for loc in servers:
        print(loc);
        
        path_resource = os.path.join("resource", loc);
        path_abfile = path_resource + ".ab";
        path_output = os.path.join("locales", loc);
        
        mkdir(path_resource);
        mkdir(path_output);
        
        #pull(loc, path_abfile);
        #unpack(path_abfile, path_resource);
        get_text(path_resource, path_output);
        

def pull(loc, outfilepath):
    path_android = servers[loc] + "/files/Android/asset_textes.ab";
    call([ADB, "pull", path_android, outfilepath]);

def unpack(infilepath, outpath):
    with open(infilepath, "rb") as f:
        bundle = unitypack.load(f);
        asset = bundle.assets[0];
        
        ab = asset.objects[1];
        for org_path, b in ab.read()["m_Container"]:
            obj = b["asset"].object;
            if obj.type == "TextAsset":
                data = obj.read();
                if isinstance(data.script, str):
                    outfilepath = os.path.join(outpath, os.path.normpath(org_path));
                    mkdir(os.path.dirname(outfilepath));
                    with open(outfilepath, "wb") as fo:
                        fo.write(data.script.encode('utf-8'));

def get_text(inpath, outpath):
    inpath = os.path.join(inpath, "assets", "resources", "dabao", "table");
    outpath = os.path.join(outpath, "table.json");
    table = OrderedDict();
    for table_file in table_files:
        table2 = {};
        filepath = os.path.join(inpath, table_file);
        key_list = table_files[table_file];
        with open(filepath, "r", encoding="utf-8") as f:
            for line in f.read().splitlines():
                items = line.split(",");
                if len(items) != 2:
                    continue;
                
                for key in key_list:
                    if items[0].startswith(key):
                        table2[items[0]] = items[1];
        for key in sorted(table2.keys()):
            table[key] = table2[key];
    table["placeholder_table"] = "";
    with open(outpath, "w", encoding="utf-8") as fo:
        fo.write(json.dumps(table, indent=2, sort_keys=False, ensure_ascii=False));
            

def mkdir(filepath):
    os.makedirs(filepath, exist_ok=True);

if __name__ == "__main__":
    extract_all();
