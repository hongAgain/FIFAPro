

function getpathes(rootpath, pathes)
    pathes = pathes or {}
    ret, files, iter = pcall(lfs.dir, rootpath)
    if ret == false then
        return pathes
    end

    for entry in files, iter do
        local next = false
        if entry ~= '.' and entry ~= '..' then
            local path = rootpath .. '/' .. entry
            local attr = lfs.attributes(path)
            if attr == nil then
                next = true
            end

            if next == false then 
                if attr.mode == 'directory' then
                    getpathes(path, pathes)
                else
                    table.insert(pathes, path)
                end
            end
        end
        next = false
    end
    return pathes
end


function dump(o)
   if type(o) == 'table' then
      local s = '{ '
      for k,v in pairs(o) do
         if type(k) ~= 'number' then k = '"'..k..'"' end
         s = s .. '['..k..'] = ' .. dump(v) .. ','
      end
      return s .. '} '
   else
      return tostring(o)
   end
end


function dump_obj(obj, key, sp, lv, st)
    sp = sp or '  '
 
    if type(obj) ~= 'table' then
        return sp..(key or '')..' = '..tostring(obj)..'\n'
    end
 
    local ks, vs, s= { mxl = 0 }, {}
    lv, st =  lv or 1, st or {}
 
    st[obj] = key or '.' -- map it!
    key = key or ''
    for k, v in pairs(obj) do
        if type(v)=='table' then
            if st[v] then -- a dumped table?
                table.insert(vs,'['.. st[v]..']')
                s = sp:rep(lv)..tostring(k)
                table.insert(ks, s)
                ks.mxl = math.max(#s, ks.mxl)
            else
                st[v] =key..'.'..k -- map it!
                table.insert(vs,
                    dump_obj(v, st[v], sp, lv+1, st)
                )
                s = sp:rep(lv)..tostring(k)
                table.insert(ks, s)
                ks.mxl = math.max(#s, ks.mxl)
            end
        else
            if type(v)=='string' then
                table.insert(vs,
                    (('%q'):format(v)
                        :gsub('\\\10','\\n')
                        :gsub('\\r\\n', '\\n')
                    )
                )
            else
                table.insert(vs, tostring(v))
            end
            s = sp:rep(lv)..tostring(k)
            table.insert(ks, s)
            ks.mxl = math.max(#s, ks.mxl);
        end
    end
 
    s = ks.mxl
    for i, v in ipairs(ks) do
        vs[i] = v..(' '):rep(s-#v)..' = '..vs[i]..'\n'
    end
    return '{\n'..table.concat(vs)..sp:rep(lv-1)..'}'
end

--get file path
function stripfilename(filename)  
    return string.match(filename, "(.+)/[^/]*%.%w+$") --*nix system  
    --return string.match(filename, “(.+)\\[^\\]*%.%w+$”) — windows  
end  

--get filename 
function strippath(filename)  
    return string.match(filename, ".+/([^/]*%.%w+)$") -- *nix system  
    --return string.match(filename, “.+\\([^\\]*%.%w+)$”) — *nix system  
end  
  
-- remove extension
function stripextension(filename)  
    local idx = filename:match(".+()%.%w+$")  
    if(idx) then  
        return filename:sub(1, idx-1)  
    else  
        return filename  
    end  
end  
  
--get extension  
function getextension(filename)  
    return filename:match(".+%.(%w+)$")  
end  

--write text file
function writeFile(fileName,text)
 local f = assert(io.open(fileName, 'w'))
 f:write(text)
 f:close()
end

