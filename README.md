# Project_Accepted-

it's a game "accepted" repository

1. unity version : 2018.3.14f1 (꼭 이걸로 할 것)

2. client ID : 994484244486-70sdlmmqitlqrog78oo8turkbr1k5fv1.apps.googleusercontent.com (이건 필요할 시에. gpgid 관련이여서 아마 필요없을 듯)

3. 안드로이드 스튜디오가 있으면 최소 Android 6.0 (Mashmallow)까지 다운. 다른 설정은 나중에 이미지로 따로 보내주겠음. 가급적 google play 관련은 다 다운받으면 됨.

4. android build tool : 현재 28.0.3 & 29.0.2로 설정되어 있음.

5. (유니티에서) unity -> preference -> external tools -> ndk 다운로드 (r16b 버젼 "android-ndk-r16b")

6. unity -> preference -> external tools -> use embedded JDK 체크

7. 자바 환경변수 문제가 발생시 링크 참고 : (맥 버젼) https://whitepaek.tistory.com/28    (윈도우 버젼) https://commin.tistory.com/4

8. build setting -> player setting -> android -> other settings -> Use exist Keystore 체크 & Browse keystore에서 user.keystore 선택 & keystore password : otk30dmg5c & 밑의 Key -> Alias : acceptedkey & Password : otk30dmg5c 

9. build failed가 혹시 발생할 때 Assets -> Play service resolver -> android resolver -> force resolve 하고 다시 시도


자잘한 오류가 워낙 많아서 다른 문제가 발생하면 그때 확인해서 해결해봐야할 듯
