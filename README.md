# Akıllı Trafik Işıkları Teknik Görev
## Nasıl Test Edilir?
### Bireysel Işık Test Etme
Sadece sahneye bir `Traffic Light` prefab'ı sürüklemeniz yeterlidir.
### Kavşak Test Etme
Boş bir `GameObject` oluşturup `JunctionGroup` component'i ekleyin. İstediğiniz sayıda ışığı bu objeye child olarak ekleyin. Devamında `JunctionGroup`'un `Lights` listesine kavşağa dahil olan bütün ışıkları ekleyin. Config `ScriptableObject` dosyası atamayı unutmayın! Atanmadığı durumda kavşak default config'i kullanır.
## Nasıl Çalışır?	
Trafik ışıkları iki şekilde çalışabilir: Bireysel (tek başına duran) ve kavşak üyesi (birden fazla ışığın bir arada olduğu, JunctionGroup sınıfı tarafından yönetilen bir kavşağın parçasıdır.
### ITrafficLightState Arayüzü
Bir trafik ışığının mevcut durumunu (yeşil, sarı, kırmızı) ve bu durumlar arasındaki geçişleri yönetmek için bir State Pattern arayüzüdür. Sadece bireysel (tek başına duran) trafik ışıkları bu arayüzü kullanır.
### TrafficLight Sınıfı
Bir trafik ışığını temsil eder. Kendi başına çalışabilir veya bir JunctionGroup'a (kavşak) üye olabilir. Eğer bireysel ise durumunu ITrafficLightState ile yönetir. Kavşak üyesiyse, state yönetimi JunctionGroup tarafından yapılır.
### JunctionGroup Sınıfı
Bir kavşaktaki birden fazla trafik ışığını merkezi olarak yönetir. Kavşaktaki ışıklar bir liste olarak tutulur. Sırasıyla bir ışık yeşil olur, süresi bitince sarıya, sonra kırmızıya döner ve sıra bir sonraki ışığa geçer. Tüm state ve zamanlama yönetimi bu sınıfta yapılır; üye ışıklar sadece komut alır.
"config" ScriptableObject özelliği kavşak ayarlarını (süreler, strateji tipi) tutar.
### TrafficLightManager Sınıfı
Tüm trafik ışıklarını ve kavşakları merkezi olarak yönetir. Tüm kavşakları ve bireysel ışıkları bulur. Simülasyonu başlatır/durdurur. Acil durumda tüm ışıkları kırmızıya çeker.
### ITrafficStrategy Arayüzü
Işıkların yeşil ve sarı sürelerini belirleyen algoritmaları soyutlar. Hem bireysel ışıklar hem de kavşaklar için farklı stratejiler uygulanabilir (ör. sabit süreli, adaptif).
### JunctionConfigSO ScriptableObject
Kavşak ayarlarını (isim, strateji tipi, süreler) Unity editöründen yapılandırmak için kullanılır.
•	Bireysel ışıklar kendi state'lerini (yeşil, sarı, kırmızı) ITrafficLightState ile yönetir.
•	Kavşak üyesi ışıklar ise state değişimlerini ve zamanlamayı tamamen JunctionGroup'tan alır; kendi başlarına state değiştirmezler.
•	Kavşaklar sırayla her ışığı yeşile geçirir, süre dolunca sarı ve kırmızıya çeker, sonra sıradaki ışığı yeşile geçirir.
•	Stratejiler ile süreler dinamik olarak belirlenebilir.