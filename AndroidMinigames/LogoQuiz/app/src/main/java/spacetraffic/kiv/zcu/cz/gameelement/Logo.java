/**
 Copyright 2010 FAV ZCU

 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

 http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.

 **/
package spacetraffic.kiv.zcu.cz.gameelement;

import org.ksoap2.serialization.KvmSerializable;
import org.ksoap2.serialization.PropertyInfo;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapPrimitive;

import java.util.Hashtable;

/**
 * Logo class.
 */
public class Logo implements KvmSerializable{

    /**
     * Image name with extension.
     */
    private String imageName;

    /**
     * Name (right answer).
     */
    private String name;

    /**
     * Logo constructor.
     * @param soapObject object
     */
    public Logo(SoapObject soapObject)
    {
        this.imageName = soapObject.getProperty("ImageName").toString();
        this.name = soapObject.getProperty("Name").toString();
    }

    /**
     * Method for get property.
     * @param arg0 which property
     * @return property
     */
    @Override
    public Object getProperty(int arg0) {
        switch(arg0){
            case 0:
                return imageName;
            case 1:
                return name;
        }
        return null;
    }

    /**
     * Method for get property count.
     * @return property count
     */
    @Override
    public int getPropertyCount() {
        return 2;
    }

    /**
     * Method for get property info.
     * @param index index
     * @param arg1 hashtable arguments
     * @param info property info
     */
    @Override
    public void getPropertyInfo(int index, @SuppressWarnings("rawtypes") Hashtable arg1, PropertyInfo info) {
        switch(index){
            case 0:
                info.type = PropertyInfo.STRING_CLASS;
                info.name = "ImageName";
                break;
            case 1:
                info.type = PropertyInfo.STRING_CLASS;
                info.name = "Name";
                break;
        }
    }

    /**
     * Setter for property (not used).
     * @param arg0
     * @param arg1
     */
    @Override
    public void setProperty(int arg0, Object arg1) {
    }

    /**
     * Getter for image name.
     * @return image name
     */
    public String getImageName() {
        return imageName;
    }

    /**
     * Getter for name.
     * @return name
     */
    public String getName() {
        return name;
    }
}
